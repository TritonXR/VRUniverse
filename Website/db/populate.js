/*
    python did a bunch of weird things with strings so here i am rewriting
    everything in javascript
*/

var sqlite3 = require('sqlite3').verbose();
const DBNAME = 'test.db';
var path = require('path');
var db = new sqlite3.Database(path.resolve( './' + DBNAME),
							sqlite3.OPEN_READWRITE, (err) => {
	if (err) console.error(err.message);
});
var fs = require('fs');
var glob = require('glob');

const planet_insert_sql = "INSERT INTO planets (name, creator, description, year, image, executable, tags) VALUES(?, ?, ?, ?, ?, ?, ?)" 
const tag_insert_sql = "INSERT or IGNORE INTO tags (tag) VALUES(?)"
const map_insert_sql = "INSERT INTO map (planet_id, tag_id) VALUES(?, ?)"

glob("../data/VRClubUniverseData/*.json", function(er, files) {
    if(er) return;

    var x = 0;
    var planets = [];
    files.forEach(function(file) {
        x++;
        planets = planets.concat(require(file)['PlanetJSON']);
        if (x == files.length) {
            populate(planets);
        }
    });
});


function populate(planets) {
    //populatePlanets(planets);
    //populateTags(planets);
    populateMapper(planets);
    db.close();
}


function populatePlanets(planets) {
    let stmt = db.prepare(planet_insert_sql)
    planets.forEach((planet) => {
        let tags = (planet['Tags']) ? planet['Tags'].toString() : "";
        stmt.run(planet.Name, planet.Creator, planet.Description, planet.Year, planet.Image, planet.Executable, tags);
    });
    stmt.finalize();
}

function populateTags(planets) {
    let stmt = db.prepare(tag_insert_sql);

    planets.forEach((planet) => {
        let tags = (planet['Tags']) ? planet['Tags'] : [];
        tags.forEach((tag) => {
            stmt.run(tag);
        });
    });

    stmt.finalize();
}

function populateMapper(planets) {
    planets.forEach((planet) => {
        let id, tagID;
        console.log('here');
        db.get('SELECT id from planets where name = ?', [planet.Name], (err, row) => {
            id = row.id;
            let tags = (planet['Tags']) ? planet['Tags'] : [];
            console.log(tags);
            tags.forEach((tag) => {
                db.get('SELECT tag_id from tags where tag = ?', [tag], (err, row) => {
                    tagID = row.tag_id;
                })
                console.log(id, tagID)
                db.run(map_insert_sql, [id, tagID]);
            });
        })



            /*for (let i = 0; i < tags.length; i++) {
                    db.get('SELECT tag_id from tags where tag = ?', [tag], (err, row) => {
                        tagID = row.tag_id;
                    })

                    if (i == tags.length - 1) {
                        console.log(id, tagID)
                        db.run(map_insert_sql, [id, tagID]);
                    }
            }*/

    }); 
}
