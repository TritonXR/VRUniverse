var sqlite3 = require('sqlite3').verbose();
var path = require('path');

var db = new sqlite3.Database(path.resolve( __dirname + './../db/universe.db'),
							sqlite3.OPEN_READONLY, (err) => {
	if (err) console.error(err.message);
	else console.log('Connected to the universe database!');
});

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

	db.serialize(() => {
	  	db.all(str, (err, rows) => {

		    if (err) {
		      console.error(err.message);
		    }
		    callback(rows);
		});
	});	
}

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