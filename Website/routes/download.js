var express = require('express');
var router = express.Router();
var fs = require("fs");

var data;

function readFiles(dirname, callback) {
    var data = {};
    var counter = 0;
    var sum;
    fs.readdir(dirname, function (err, filenames) {
        console.log("doing read dir");
        if (err) {
            console.log(err);
            return;
        }
        console.log(typeof filenames);
        var array = filenames
            .filter(function (filename) {
                return filename.substr(-5) === '.json';
            });
        sum = array.length;
        array.forEach(function (filename) {
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
                if (counter === sum) {
                    callback(data)
                }
            });
        });

    });

} 


/* GET users listing. */

router.get('/', function (req, res, next) {
    readFiles('./data/VRClubUniverseData/', function (data) {
        console.log(data);
        data = data;
        res.render('download', {
            json: JSON.stringify(data)
        });
    });
});

router.post('/', function (req, res, next) {
    console.log("posting");
    if (!data) {
        readFiles('./data/VRClubUniverseData/', function (data) {
            data = data;
        });
    }
    let incoming_data = JSON.parse(req);
    console.log(incoming_data);





    
    
})
module.exports = router;