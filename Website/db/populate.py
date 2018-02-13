import sqlite3
import json

year = 2016
data = json.load(open('../data/VRClubUniverseData/' + str(year) + '.json'))
conn = sqlite3.connect('universe.db')

planet_insert_sql = "INSERT INTO planets (name, creator, description, year, image, executable) VALUES(?, ?, ?, ?, ?, ?)" 
tag_insert_sql = "INSERT or IGNORE INTO tags (tag, tag_id) VALUES(?, ?)"

def convert(input):
    if isinstance(input, dict):
        return dict((convert(key), convert(value)) for key, value in input.iteritems())
    elif isinstance(input, list):
        return [convert(element) for element in input]
    elif isinstance(input, unicode):
        return input.encode('utf-8')
    else:
        return input

i = 25
convert(data)
for planet in data['PlanetJSON']:
    for tag in planet['Tags']:
        conn.execute(tag_insert_sql, (tag, i))
        conn.commit()
        i += 1 
    

