var express = require('express');
var router = express.Router();
var fs = require("fs");

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
    
    readFiles('./data/VRClubUniverseData/', function (data) {
        console.log(data);
        res.render('projects', {
            json:  JSON.stringify(data)
        });      
    });
});


  



//{ "data" : JSON.stringify(data) }
module.exports = router;
