var express = require('express');
var router = express.Router();
var fs = require("fs");
var archiver = require("archiver"); //node-archiver
var JSZip = require("jszip");
var vive = require('./vive-db.js');
var async = require('async');
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
    res.render('download');
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
});

// testing using JSZip
router.get('/file', function (req, res, next){
    var zip = new JSZip();
    zip.folder("./data/VRClubUniverseData/Vive/2018/StarshotExpress");
    zip.generateAsync({type:"blob"}).then(function(content){
        SaveAs(content, 'result.zip');
    });
});

// testing using node archiver
router.get('/zip', function (req, res, next) {
    // download filename == 'projects.zip'
    res.attachment('projects.zip');

    // define platform --> either Vive or Oculus
    var platform = "Vive";

    // filter projects choose to download
    var project_map = new Map();
    project_map.set("StarshotExpress", 1);
    project_map.set("Mars2020", 1);
    project_map.set("UltimateDragonMechVsMonsterShowdown", 1);
    project_map.set("Vivace", 1);

    // initialize zip
    var zip = archiver('zip');
    zip.pipe(res);

    // default path of project data.
    var dir = "./data/VRClubUniverseData/" + platform + "/";

    // arrays store years and project names.
    // somehow fs will interrupt database query, so divide into 2 steps
    // get projects, then prepare zip.
    var projects = [];
    var years = [];
    async.series([
        function(callback){
            vive.getAllProjects(function(data) {
                for(i in data){
                    if(project_map.get(data[i].executable)) {
                        projects.push(data[i].executable); // Thankfully, executable is same as the
                                                           // name used in the path
                        years.push(data[i].year);
                    }
                }
                callback();
            });
        },
        function(callback){
            for(var i = 0; i < projects.length; i++) {
                dfs(years[i] + "/" + projects[i] + "/");
            }
            callback();
        },
        function(callback) {
            zip.finalize(function (err){
                return res.end();
            });
        }
    ]);

    // this function stacks subdirectory into zip in its original hierachy.
    function dfs(filepath) {
        // console.log(dir + filepath);
        fs.readdirSync(dir + filepath).forEach(file => {
            var path = filepath + file;
            if(fs.lstatSync(dir + path).isDirectory()){
                dfs(path + "/");
            }
            else {
                // console.log("file: " + path);
                zip.append(fs.createReadStream(dir+path), {name: path});
            }
        });
    }
});

module.exports = router;