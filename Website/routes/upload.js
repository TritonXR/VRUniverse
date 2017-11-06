var express = require('express');
var router = express.Router();

/* GET users listing. */

router.get('/', function(req, res, next) {
  res.render('upload');
});

router.post('/', function(req, res, next) {
	res.send("lalalalalala");
});

module.exports = router;