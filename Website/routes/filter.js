var express = require('express');
var router = express.Router();
var db = require('./db.js');

String.prototype.isEmpty = function() {
    return (this.length === 0 || !this.trim());
};


router.get('/*', function(req, res, next) {
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
