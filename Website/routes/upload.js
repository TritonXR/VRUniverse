var express = require('express');
var router = express.Router();
var fs = require('fs');
var multer = require('multer');

var storage = multer.diskStorage({
    destination: function(req, file, cb){
        cb(null, 'data/upload')
    },
    filename: function(req,file,cb){
        cb(null, file.originalname)
    }
});

var upload = multer({storage:storage});

var uploadFile = function (filedest, filename){
    var storage = multer.diskStorage({
        destination: function(req, file, cb){
            cb(null, filedest)
        },
        filename: function(req,file,cb){
            cb(null, filename)
        }
    });

    var upload = multer({storage:storage}).single('file');
    return upload;
}

/* GET upload form. */
router.get('/', function(req, res, next) {
    res.render('upload');
});


router.post('/', upload.any(), function(req, res) {
    var year = req.body.year;
    var projectname = req.body.project;
    var upload_image = '';
    var upload_exec = '';

    var json_dir = "./data/VRClubUniverseData/";
    var exec_dir = "./data/VRClubUniverseData/VR_Demos/";
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

    /*
    if(req.file.image) {
        var upload = uploadFile(exec_dir + year + '/' + projectname, projectname + '_Image.jpg');
        upload(req,res,function(err){
            if(err){
                console.log(err);
            } else {
                res.send(req.file.image);
                upload_image = projectname + '_Image.jpg';
            }
        });
    };
    if(req.file.exec) {
        var upload = uploadFile(exec_dir + year + '/' + projectname, projectname + '.exe');
        upload(req,res,function(err){
            if(err){
                console.log(err);
            } else {
                res.send(req.file.exec);
                upload_exec = projectname + '.exe';
            }
        });
    }
    */
    var tags = req.body.tags;
    var tag_arr = tags.split(",");
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

    res.send("haha");
});

module.exports = router;
