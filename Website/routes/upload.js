var express = require('express');
var router = express.Router();
var fs = require('fs');
var path = require('path');
var multer = require('multer');
var vive = require('./vive-db.js');
var oculus = require('./oculus-db.js');
var unzip = require('unzip');
var yauzl = require('yauzl');

var storage = multer.diskStorage({
    destination: function(req, file, cb){
        cb(null, 'tmp/')
    },
    filename: function(req,file,cb){
        cb(null, file.originalname)
    }
});

var upload = multer({storage:storage});


router.use( function(req, res, next) {

    if (!req.isAuthenticated()) {
        if (req.method == "GET") {
            res.redirect('/user/signin');
        } else {
            res.json({
                status: false,
                msg: "logout"
            })
        }
    } else {
        next();
    }
});

/* GET upload form. */
router.get('/', function(req, res, next) {
    vive.getAllTags((tags) => {
        //tags = tags.map(x => x['tag'].trim());
        console.log(tags);
        res.render('upload', {"tags" : tags});
    });
});


router.post('/', upload.any(), function(req, res) {
    var year = req.body.year;
    var projectname = req.body.project;
    var upload_image = '';
    var upload_exec = '';
    var platform = req.body.platform;

    if (!validateYear(year)) {
        res.render('universe_err', {err: 'Please Input the Proper Year in the Range 2016-Current Year!'});
        return;
    }

    if (!platform) {
        res.render('universe_err', {err: 'Please Fill in All Fields!'});
        return;
    }    

    if (platform == 'HTC Vive') {
        platform = 'Vive';
        var db = vive;
    }
    else if (platform == 'Oculus Rift') {
        platform = 'Oculus'
        var db = oculus;
    }

    var json_dir = "./data/VRClubUniverseData/" + platform + '/';
    var exec_dir = "./data/VRClubUniverseData/" + platform + '/';
    if(!fs.existsSync(exec_dir + year))
    {
        fs.mkdirSync(exec_dir + year);
        var options = options || {};
        options.flag = 'wx';
        var content = { PlanetJSON: []};
        fs.writeFile(json_dir + year + '.json', JSON.stringify(content), options, function(err) {
            if(err){
                console.log(err);
            }
        });

    }
    if(!fs.existsSync(exec_dir + year + '/' + projectname)) {fs.mkdirSync(exec_dir + year + '/' + projectname);}

    findFile('.jpg', function(filename){
        if(filename)
        {
            fs.rename(filename,
                exec_dir + year + '/' + projectname + '/'+ projectname + '_Image.jpg',
                function(err){
                    if(err){
                        console.log(err);
                    }
                });
            upload_image = projectname + '_Image.jpg';
        }
    });
    findFile('.jpeg', function(filename){
        if(filename)
        {
            fs.rename(filename,
                exec_dir + year + '/' + projectname + '/'+ projectname + '_Image.jpg',
                function(err){
                    if(err){
                        console.log(err);
                    }
                });
            upload_image = projectname + '_Image.jpg';
        }
    });
    findFile('.png', function(filename){
        console.log("here");
        if(filename)
        {
            fs.rename(filename,
                exec_dir + year + '/' + projectname + '/'+ projectname + '_Image.jpg',
                function(err){
                    if(err){
                        console.log(err);
                    }
                });
            upload_image = projectname + '_Image.jpg';
        }
    });
    findFile('.gif', function(filename){
        if(filename)
        {
            fs.rename(filename,
                exec_dir + year + '/' + projectname + '/'+ projectname + '_Image.jpg',
                function(err){
                    if(err){
                        console.log(err);
                    }
                });
            upload_image = projectname + '_Image.jpg';
        }
    });

    findFile('.zip', function(filename){
        if(filename)
        {
            // fs.rename(filename,
            //     exec_dir + year + '/' + projectname + '/'+ projectname + '.exe',
            //     function(err){
            //         if(err){
            //             console.log(err);
            //         }
            //     });
            // upload_exec = projectname;

            fs.createReadStream(filename).pipe(unzip.Extract({ path: exec_dir + year + '/' + projectname + '/'+ projectname + '.exe' }));
            upload_exec = projectname;
        }
    });

/*
    findFile('.zip', (filename) => {
        yauzl.open(filename, {lazyEntries: true}, (err, zipfile) => {
            if(err) throw err;
            zipfile.readEntry();
            zipfile.on("entry", (entry) => {
                const regex = /\/$/;
                if(regex.test(entry.fileName)) {
                    zipfile.readEntry();
                } else {
                    zipfile.openReadStream(entry, (err, readStream) => {
                        if(err) throw err;
                        readStream.on("end", () => {
                            zipfile.readEntry();
                        });
                        readStream.pipe();
                    })
                }
            })
        });
    });
    */

    var tags = req.body.tags;
    var tag_arr = tags;
    var proj = {
        Name: projectname,
        Creator: req.body.members,
        Description: req.body.desc,
        Year: year,
        Tags: tag_arr,
        Show : true,
        Image: upload_image,
        Executable: upload_exec
    };

    var content = null;
    fs.readFile(json_dir + year + '.json', 'utf-8', function(err, file_content)
    {
        if(err) {
            console.log(err);
        } else {
            content = JSON.parse(file_content);
            content['PlanetJSON'].push(proj);
            if(content){
                fs.writeFile(json_dir + year + '.json', JSON.stringify(content, null, 2), function (err) {
                    if(err){
                        console.log(err);
                    }
                });
            }
        }
    });

    db.createEntry(proj, function(status) {
        res.send('Successfully Uploaded Project!');
    });

});

function findFile(extension, cb){
    var file = fs.readdirSync('tmp/');
    for(var i = 0; i < file.length; i++){
        var filename = path.join('tmp/', file[i]);
        if(filename.indexOf(extension) >= 0){
            cb(filename);
        }
    }
}

/**
 * Return false for improper year input, true otherwise.
 * @param {string} year the user input year
 */
function validateYear(year) {
    let regex = /^[0-9]+$/;
    if(year.length == 4) {
        if((year != "") && (!regex.test(year))) {
            return false;
        }
        let curr_year = new Date().getFullYear();
        year = parseInt(year);
        if((year < 2016) || (year > curr_year)) {
            return false;
        }
    } else {
        return false;
    }
    return true;
}

module.exports = router;
