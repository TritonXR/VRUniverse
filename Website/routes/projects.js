var express = require('express');
var router = express.Router();
var fs = require("fs");



function readFiles(dirname, callback) {
    
    var data = {};
    var counter = 0;
    fs.readdir(dirname, function (err, filenames) {
        console.log("doing read dir");
        if (err) {
            console.log(err);
            return;
        }
        
        filenames
            .filter(function (filename) {
                return filename.substr(-5) === '.json';
            })
            .forEach(function (filename) {
                console.log(dirname + filename);
                fs.readFile(dirname + filename, 'utf-8', function (err, content) {
                    if (err) {
                        onError(err);
                        return;
                    }
                    console.log("doing read file");
                    var year = filename.split(".")[0];
                    data[year] = content;
                    counter++;
                    console.log("counter is " + counter);
                    if (counter === 2) {
                        callback(data)
                    }    
                });
            });   
    
    });
    
} 



/* GET users listing. */



/*
router.get('/', function (req, res, next) {
    var data = {};
    return Promise.try(function () {
        console.log("executing the main function");
        return readFiles('./data/VRClubUniverseData/'), function (filename, content) {
            var year = filename.spilt(".")[0];
            data[year] = content;
        }
    }).then(function () {
        console.log(data);
        res.render('projects', {
            "data": JSON.stringify(data)
        });
    });
}); */
router.get('/', function (req, res, next) {
    //return Promise.try(function () {
    readFiles('./data/VRClubUniverseData/', function (data) {
        console.log("data is " + data[2016]);
        res.render('projects', {
            //"data": JSON.stringify(data)
            data
        });      
    });
});
  
  



//{ "data" : JSON.stringify(data) }
module.exports = router;
