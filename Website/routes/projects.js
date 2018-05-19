var express = require('express');
var router = express.Router();
var fs = require("fs");
var db = require('./db.js');

router.get('/', function (req, res, next) {

    db.getAllTags(function(tags) {
        db.getAllProjects(function(data) {
            res.render('projects', {
                json: JSON.stringify(data),
                tags : JSON.stringify(tags),
                tagArray : null
            });
        });
    });
});

module.exports = router;
