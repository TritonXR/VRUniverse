var express = require('express');
var router = express.Router();
var db = require('./db.js');

String.prototype.isEmpty = function() {
    return (this.length === 0 || !this.trim());
};


router.get('/', function(req, res, next) {
	res.send('Welcome To The Filter Page');
});

/* GET tag page. */
router.get('/*', function (req, res, next) {

	var tagArr = []
	var url = req.params[0].split('/');

	for (let i in url) {
		if (!url[i].isEmpty()) tagArr.push(url[i]);
	}

	db.getProjectsFromTags(tagArr, function(query) {

		if (query == null || query.length === 0) {
			res.send("Cant Find Any Projects With That Tag!");
		}
		else {
			for (let i = 0; i < query.length; i++) query[i] = JSON.stringify(query[i]);

			res.render('filter', {json : JSON.stringify(query), tag : tagArr});
		}
	});
});

module.exports = router;
