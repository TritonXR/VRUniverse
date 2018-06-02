var express = require('express');
var router = express.Router();
var vive = require('./vive-db.js');
var oculus = require('./oculus-db.js');

String.prototype.isEmpty = function() {
    return (this.length === 0 || !this.trim());
};


router.get('/vive', function(req, res, next) {
	let db = vive;
	let obj = req.query;
	var tagArr = []

	for (let key in obj) {
		tagArr.push(key);
	}

	console.log(tagArr);

	if (tagArr.length == 0) {
		res.redirect('/projects');
		return;
	}

	db.getAllTags(function(data) {
		db.getProjectsFromTags(tagArr, function(query) {

			if (query == null || query.length === 0) {
				res.render('project-platform', {json : JSON.stringify(query), 
										tagArray : [],
										tags : JSON.stringify(data),
										platform: 'Vive'
										});
			}
			else {
				res.render('project-platform', {json : JSON.stringify(query), 
										tagArray : tagArr,
										tags : JSON.stringify(data),
										platform: 'Vive'
										});
			}
		});		
	});

});


router.get('/Oculus', function(req, res, next) {
	let db = oculus;
	let obj = req.query;
	var tagArr = []

	for (let key in obj) {
		tagArr.push(key);
	}

	console.log(tagArr);

	if (tagArr.length == 0) {
		res.redirect('/projects');
		return;
	}

	db.getAllTags(function(data) {
		db.getProjectsFromTags(tagArr, function(query) {

			if (query == null || query.length === 0) {
				res.render('project-platform', {json : JSON.stringify(query), 
										tagArray : [],
										tags : JSON.stringify(data),
										platform: 'Oculus'
										});
			}
			else {
				res.render('project-platform', {json : JSON.stringify(query), 
										tagArray : tagArr,
										tags : JSON.stringify(data),
										platform: 'Oculus'
										});
			}
		});		
	});

});

/* GET tag page. */

module.exports = router;
