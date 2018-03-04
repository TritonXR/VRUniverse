var express = require('express');
var router = express.Router();
var fs = require("fs");
var db = require('./db.js');


function readFiles(dirname, callback) {
    var data = {};
    var counter = 0;
    var sum;
    fs.readdir(dirname, function (err, filenames) {
        if (err) {
            console.log(err);
            return;
        }
        var array = filenames
            .filter(function (filename) {
                return filename.substr(-5) === '.json';
            });
        sum = array.length;
        array.forEach(function (filename) {
            fs.readFile(dirname + filename, 'utf-8', function (err, content) {
                if (err) {
                    onError(err);
                    return;
                }
                var year = filename.split(".")[0];
                data[year] = content;
                counter++;
                if (counter === sum) {
                    callback(data)
                }    
            });
        });   
    });  
} 


router.get('/', function (req, res, next) {

    db.getAllTags(function(tags) {
        db.getAllProjects(function(data) {
            res.render('download', {
                json: JSON.stringify(data),
                tags : JSON.stringify(tags),
                tagArray : null
            });
        });
    });
});

function updateShow(projs, callback) {
    for (index in projs["PlanetJSON"]) {
        if (projs["PlanetJSON"][index].Name != key) {
            projs["PlanetJSON"][index].Show = false;
        }
        //console.log("AAAAA " + JSON.stringify(projs["PlanetJSON"][index]));
        if (index == projs["PlanetJSON"].length - 1) {
            console.log("callback!!!!");
            callback(projs);
        }
    }
    
}

module.exports = router;
