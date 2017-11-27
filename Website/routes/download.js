var express = require('express');
var router = express.Router();
var fs = require("fs");
var fs_extra = require("node-fs-extra");
var data = {};
var years = [];

function readFiles(dirname, callback) {
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
                years.push(year);
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
    readFiles('./data/VRClubUniverseData/', function (data2) {
        console.log(data);
        data = data2;
        res.render('download', {
            json: JSON.stringify(data)
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

router.post('/', function (req, res, next) {
    if (!data) {
        console.log("data is empty!");
        years = [];
        readFiles('./data/VRClubUniverseData/', function (data2) {
            data = data2;
        });
    }

    fs_extra.copy('./data', './download', function (err) {
        if (err) {
            console.error(err);
        } else {
            console.log("success!");
        }
    });

    let incoming_data = req.body;
    console.log(years);
    for (year_index in years) {
        var year = years[year_index];
        var projects = JSON.parse(data[year]);
        for (index in projects["PlanetJSON"]) {
            var show = false;
            for (key in incoming_data) {
                show = show || (projects["PlanetJSON"][index].Name == key);
            }
            if (!show) {
                projects["PlanetJSON"][index].Show = false;
            }
        }
        fs.writeFile('./data/VRClubUniverseData/' + year + ".json", JSON.stringify(projects), function (err) {
            if (err) return console.log(err);
                console.log("writing file success!")
        });
    }
    res.send(req.body);
})

module.exports = router;