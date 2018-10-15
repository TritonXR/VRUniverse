var express = require('express');
var router = express.Router();
var fs = require("fs");
var JSZip = require("jszip");
var FileSaver = require('file-saver');
var fs_extra = require("node-fs-extra");
//var db = require('./db.js');


var data = {};
var fulldata = {};
var years = [];

function readFiles(dirname, callback) {
    years = [];
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
                years.push(year);
                data[year] = content;
                JSON.parse(content).PlanetJSON.forEach(function(elem) {
                    fulldata[elem.Name] = elem;
                });
                counter++;

                if (counter === sum) {
                    callback(data)
                }
            });
        });

    });

} 


/* GET users listing. */
router.get('/vive', function (req, res, next) {
    var zip = new JSZip();
    zip.folder('./data/VRClubUniverseData/');
    zip.generateAsync({type:"uint8array"}).then(function(content){
        SaveAs(content, 'text.zip');
    });

    res.redirect('../download');
});

router.get('/', function (req, res, next) {
    //readFiles('./data/VRClubUniverseData/', function (data) {
        res.render('download');      
    //});
});

router.post('/', function (req, res, next) {

    let filtered = {};
    /*if (!data) {
        console.log("data is empty!");
        readFiles('./data/VRClubUniverseData/', function (data2) {
            data = data2;
        });
    }

    fs_extra.copy('./data', './download', function (err) {
        if (err) {
            console.error(err);
        } else {
            console.log("success!");

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
                fs.writeFile('./download/VRClubUniverseData/' + year + ".json", JSON.stringify(projects), function (err) {
                    if (err) return console.log(err);
                    console.log("writing file success!")
                });
            }
            res.render('download_success');
        }
    });*/

    /*years.forEach((year) => {
        console.log(JSON.parse(data[year]));
    });*/
    
    Object.keys(req.body).forEach(function(k) {
        if (filtered.hasOwnProperty(fulldata[k].Year)) {
            filtered[fulldata[k].Year].PlanetJSON.push(fulldata[k]);
        }
        else {
            filtered[fulldata[k].Year] = {'PlanetJSON' : [fulldata[k]]}
        }
    });

    res.json({'whahahahaha' : 'hahahaha'});    
})

module.exports = router;