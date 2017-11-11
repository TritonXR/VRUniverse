var express = require('express');
var router = express.Router();
var formidable = require("formidable");
var fs = require('fs');

// fs.readFile('../data/VRClubUniverseData/VR_Demos/');


/* GET users listing. */

router.get('/', function(req, res, next) {
    res.render('upload');
});

router.post('/', function(req, res, next) {

    processForm(req, res, function (data) {
        res.send(data);
    })
});

function processForm(req,res, callback){

    var form  = formidable.IncomingForm();

    form.parse(req, function(err, fields, files) {
        // mkdir if not exist
        var year = 2016;
        var json_dir = "./data/VRClubUniverseData/";
        var exec_dir = "./data/VRClubUniverseData/VR_Demos/";
        //if(!fs.exists(exec_dir + year)) {fs.mkdir(exec_dir + year);}

        // parse texts
        var projectname = "something";
        var content = null;
        console.log("here");
        fs.readFile(json_dir + year + '.json', 'utf-8', function(err, file_content)
        {
            if(err) {
                console.log(err);
                return;
            }
            content = JSON.parse(file_content);
            console.log(content);

            if (content != nul) {
                callback(content)
            }
        });

        // parse non-text stuff
        /* var image = req.body.picture;
        fs.writeFile(exec_dir + year + '/' + projectname + '/' , image, function(err){
            if(err){
                console.log(err);
            }
        });
        var exec = req.body.exec;
        fs.writeFile(exec_dir + year + '/' + projectname, exec, function(err){
            if(err){
                console.log(err);
            }
        }) */
    });
}

module.exports = router;