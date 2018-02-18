import sqlite3
import json

year = 2017
data = json.load(open('../data/VRClubUniverseData/' + str(year) + '.json'))
conn = sqlite3.connect('universe.db')

planet_insert_sql = "INSERT INTO planets (name, creator, description, year, image, executable) VALUES(?, ?, ?, ?, ?, ?)" 
tag_insert_sql = "INSERT or IGNORE INTO map (tag, tag_id) VALUES(?, ?)"
map_insert_sql = "INSERT INTO map (planet_id, tag_id) VALUES(?, ?)"

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

#this is adding the data into the mapper table
"""for planet in data['PlanetJSON']:
    id = None;
    for i in conn.execute('SELECT id from planets where name = ?', (convert(planet['Name']),)):
        id = i[0];
    for tag in planet['Tags']:
        tag_id = None;
        for i in conn.execute('SELECT tag_id from tags where tag = ?', (convert(tag),)):
            tag_id = i[0]
        conn.execute(map_insert_sql, (id, tag_id));
    conn.commit(); """


# this is how u select projects with tag name
for row in conn.execute('SELECT DISTINCT * from planets where id in (select planet_id from map where tag_id in (select tag_id from tags where tag = ? or ?))', ('Red', 'HTC Vive')):
    print(row)
