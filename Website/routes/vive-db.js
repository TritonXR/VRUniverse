var sqlite3 = require('sqlite3').verbose();
var path = require('path');
var db_path = (process.env.UNIVERSE_DB_DEV) ? 'test_vive.db' : 'vive.db';
var db = new sqlite3.Database(path.resolve( __dirname + '/../data/VRClubUniverseData/Vive/' + db_path),
							sqlite3.OPEN_READWRITE, (err) => {
	if (err) console.error(err.message);
	else console.log('Connected to the ' + db_path + ' database!');
});

exports.getAllProjects = (callback) => {
	db.serialize(() => {
	  	db.all(`SELECT * from planets`, 
	  			(err, rows) => {

		    if (err) {
		      console.error(err.message);
		    }
		    callback(rows);
		});
	});
}

exports.getAllTags = (callback) => {
	db.serialize(() => {
	  	db.all(`SELECT DISTINCT tag from tags`, 
	  			(err, rows) => {

		    if (err) {
		      console.error(err.message);
		    }
		    callback(rows);
		});
	});
}

exports.getProjectsFromTag = (tag, callback) => {
	db.serialize(() => {
	  	db.all(`SELECT DISTINCT * from planets where id in 
	  				(select planet_id from map where tag_id in 
	  					(select tag_id from tags where tag = '${tag}'))`, 
	  			(err, rows) => {

		    if (err) {
		      console.error(err.message);
		    }
		    callback(rows);
		});
	});
}

exports.getProjectsFromTags = (tags, callback) => {

	var baseStr = `SELECT DISTINCT * from planets where id in 
	  				(select planet_id from map where tag_id in 
	  					(select tag_id from tags where tag = '${tags[0]}'))`;

	var str = baseStr;

	for (let i = 1; i < tags.length; i++) {
		str += ` INTERSECT SELECT DISTINCT * from planets where id in 
	  				(select planet_id from map where tag_id in 
	  					(select tag_id from tags where tag = '${tags[i]}'))`
	}

	console.log(str);

	db.serialize(() => {
	  	db.all(str, (err, rows) => {

		    if (err) {
		      console.error(err.message);
		    }
		    callback(rows);
		});
	});	
}

exports.createEntry = (json, callback) => {

	let vals = [json.Name, json.Creator , json.Description , json.Year, 
				json.Image, json.Executable, json.Tags.toString()];

	let createPlanetEntry = `INSERT INTO planets (name, creator, description, 
								year, image, executable, tags) 
									VALUES ( ` + vals.map(val => ' ? ').join(' , ') + " );";

	let createTagsEntry = `INSERT OR IGNORE INTO tags (tag) VALUES ` + json.Tags.map(tag => '(?)').join(' , ');  

	let createMapperEntry = `INSERT INTO map (planet_id, tag_id) VALUES `;

	let lastID = null;

	db.serialize(() => {
	  	db.run(createPlanetEntry, vals, (err) => {

		    if (err) {
		      console.error(err.message);
		    }
		    else {
		    	db.all(`SELECT id from planets where name= '${json.Name}'`, function (err, rows) {
		    		if (err) {
		    			console.error(err.message);
		    		}
		    		else lastID = rows[0].id;
		    	});
		    }

		});
		db.run(createTagsEntry, json.Tags, (err) => {
			if (err) {
		      console.error(err.message);
		    }
		});
		this.getTagIDsFromTags(json.Tags, function(tags) {
			createMapperEntry += tags.map(val => `(${lastID}, ${val.tag_id})`).join(' , ');
			console.log(createMapperEntry);
			db.serialize(() => {
				db.run(createMapperEntry, function(err) {
					if (err) {
						console.error(err.message);
					}
				})
			});
		});

		callback({success : true})
	});
}

exports.getTagIDsFromTags = (tags, callback) => {
	let sql = `SELECT tag_id from tags where tag in (` + tags.map(tag => '(?)').join(' , ') + ");";
	db.serialize(() => {
		db.all(sql, tags, (err, rows) => {
			if (err) {
				console.error(err.message);
			}
			else {
				callback(rows);
			}
		});
	});
}

exports.createTag = (tag, callback) => {
	let sql = `SELECT tag_id from tags where tag in (` + tags.map(tag => '(?)').join(' , ') + ");";
	db.serialize(() => {
		db.all(sql, tags, (err, rows) => {
			if (err) {
				console.error(err.message);
			}
			else {
				callback(rows);
			}
		});
	});
}

exports.removeEntry = (id, callback) => {

	db.serialize(() => {
		db.all("DELETE from planets where id = '" + id + "'", function(err) {
			if (err) {
				console.error(err.message);
			}
		})
		db.all("DELETE from map where planet_id = '" + id + "'", function(err) {
			if (err) {
				console.error(err.message);
			}
			callback(true);
		})
	})

}

//untested
exports.getTagsFromProjectName = (name, callback) => {
	var str = `SELECT DISTINCT tag from tags where tag_id in 
				(select tag_id from map where planet_id in
				(select planet_id from planets where name = '${name}'))`

	db.serialize(() => {
	  	db.all(str, (err, rows) => {

		    if (err) {
		      console.error(err.message);
		    }
		    callback(rows);
		});
	});	
}

