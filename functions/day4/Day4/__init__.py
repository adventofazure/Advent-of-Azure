import logging
import re
import azure.functions as func

# Passport has to have following fields: 
# byr (Birth Year)
# iyr (Issue Year)
# eyr (Expiration Year)
# hgt (Height)
# hcl (Hair Color)
# ecl (Eye Color)
# pid (Passport ID)
# This field is optional
# cid (Country ID)
def has_mandatory_fields(passport: list):
    mandatory_fields = ['byr', 'iyr', 'eyr', 'hgt', 'hcl', 'ecl', 'pid']
    has_fields = True
    for mandatory_field in mandatory_fields:
        fields_found = [field for field in passport if field[0] == mandatory_field]
        if len(fields_found) == 0:
            logging.info('Field ' + mandatory_field + ' not found')
            has_fields = False
            break
    return has_fields

def has_valid_values(passport: list):
    has_valid_values = True
    for field in passport:
        field_key = field[0]
        field_value = field[1]
        if field_key == 'byr':
            byr = int(field_value)
            if byr < 1920 or byr > 2002:
                has_valid_values = False
                break
        if field_key == 'iyr':
            iyr = int(field_value)
            if iyr < 2010 or iyr > 2020:
                has_valid_values = False
                break
        if field_key == 'eyr':
            eyr = int(field_value)
            if eyr < 2020 or eyr > 2030:
                has_valid_values = False
                break
        if field_key == 'hgt':
            hgt = field_value
            if not re.match('^(1([5-8][0-9]|9[0-3])cm|(59|6[0-9]|7[0-6])in)$', hgt):
                has_valid_values = False
                break
        if field_key == 'hcl':
            hcl = field_value
            if not re.match('^#([0-9]|[a-f]){6,6}$', hcl):
                has_valid_values = False
                break
        if field_key == 'ecl':
            ecl = field_value
            if not re.match('^(amb|blu|brn|gry|grn|hzl|oth)$', ecl):
                has_valid_values = False
                break
        if field_key == 'pid':
            pid = field_value
            if not re.match('^[0-9]{9,9}$', pid):
                has_valid_values = False
                break
            
    return has_valid_values

def field_to_tuple(field: str):
    colon_index = field.index(':')
    key = field[:colon_index]
    value = field[colon_index+1:]
    return (key, value)

def solve(input: str, part: int):
    passports = input.split(',,')
    valid_passports = 0
    for passport in passports:
        line = passport.replace(',', ' ')
        fields = line.split(' ')
        field_tuples = list(map(field_to_tuple, fields))
        if part == 1:
            if has_mandatory_fields(field_tuples):
                valid_passports = valid_passports + 1
        elif part == 2:
            if has_mandatory_fields(field_tuples) and has_valid_values(field_tuples):
                valid_passports = valid_passports + 1
    return valid_passports

def main(req: func.HttpRequest) -> func.HttpResponse:
    logging.info('Python HTTP trigger function processed a request.')

    input = req.params.get('input')
    if not input:
        try:
            req_body = req.get_json()
        except ValueError:
            pass
        else:
            input = req_body.get('input')

    if input:
        return func.HttpResponse(f"Part 1: " + str(solve(input, 1)) + "\nPart2: " + str(solve(input, 2)))
    else:
        return func.HttpResponse(
             "Day 4. No input given.",
             status_code=200
        )
