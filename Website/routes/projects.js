var express = require('express');
var router = express.Router();
var fs = require("fs");
var vive = require('./vive-db.js');
var oculus = require('./oculus-db.js');


router.get('/', function (req, res, next) {
    res.render('projects');
});


router.get('/Vive', function (req, res, next) {

    vive.getAllTags(function(tags) {
        vive.getAllProjects(function(data) {
            res.render('project-platform', {
                json: JSON.stringify(data),
                tags : JSON.stringify(tags),
                tagArray : null,
                platform: 'Vive'
            });
        });
    });
});

router.get('/Oculus', function (req, res, next) {

    oculus.getAllTags(function(tags) {
        oculus.getAllProjects(function(data) {
            res.render('project-platform', {
                json: JSON.stringify(data),
                tags : JSON.stringify(tags),
                tagArray : null,
                platform: 'Oculus'
            });
        });
    });
});


module.exports = router;
