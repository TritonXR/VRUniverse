var express = require('express');
var router = express.Router();
var vive = require('./vive-db.js');
var oculus = require('./oculus-db.js');

String.prototype.isEmpty = function() {
    return (this.length === 0 || !this.trim());
};


router.get('/Vive/*', function(req, res, next) {
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
				res.render('projects', {json : JSON.stringify(query), 
										tagArr : [],
										tags : JSON.stringify(data)
										});
			}
			else {
				res.render('projects', {json : JSON.stringify(query), 
										tagArr : tagArr,
										tags : JSON.stringify(data)
										});
			}
		});		
	});

});


router.get('/Oculus/*', function(req, res, next) {
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
				res.render('projects', {json : JSON.stringify(query), 
										tagArr : [],
										tags : JSON.stringify(data)
										});
			}
			else {
				res.render('projects', {json : JSON.stringify(query), 
										tagArr : tagArr,
										tags : JSON.stringify(data)
										});
			}
		});		
	});

});

/* GET tag page. */

module.exports = router;
