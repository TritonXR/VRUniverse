import sqlite3
import json

years = [str(2016), str(2017)];

planet_insert_sql = "INSERT INTO planets (name, creator, description, year, image, executable, tags) VALUES(?, ?, ?, ?, ?, ?, ?)" 
tag_insert_sql = "INSERT or IGNORE INTO tags (tag) VALUES(?)"
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

convert(data)

def populatePlanets():
    for planet in data['PlanetJSON']:
        conn.execute(planet_insert_sql, (planet['Name'], planet['Creator'], planet['Description'], planet['Year'], planet['Image'], planet['Executable'], str(planet['Tags'],)));
        conn.commit();

def populateTags():
    for planet in data['PlanetJSON']:
        for tag in planet['Tags']:
            conn.execute(tag_insert_sql, (tag, ));
            conn.commit();
            

def populateMapper():
    for planet in data['PlanetJSON']:
        id = None;
        for i in conn.execute('SELECT id from planets where name = ?', (convert(planet['Name']),)):
            id = i[0];
        for tag in planet['Tags']:
            tag_id = None;
            for i in conn.execute('SELECT tag_id from tags where tag = ?', (convert(tag),)):
                tag_id = i[0]
            conn.execute(map_insert_sql, (id, tag_id));
        conn.commit();

def populateMapperWithYears():
    for planet in data['PlanetJSON']:
        id = None;
        for i in conn.execute('SELECT id from planets where name = ?', (convert(planet['Name']),)):
            id = i[0];

            tag_id = None;
            for i in conn.execute('SELECT tag_id from tags where tag = ?', (year,)):
                tag_id = i[0]
            conn.execute(map_insert_sql, (id, tag_id));
        conn.commit();

for year in years:    
    data = json.load(open('../data/VRClubUniverseData/' + year + '.json'))
    conn = sqlite3.connect('universe.db')

    #call methods here


    conn.commit();

