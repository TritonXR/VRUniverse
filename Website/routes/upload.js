var express = require('express');
var router = express.Router();
var fs = require('fs');
var path = require('path');
var multer = require('multer');

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
    findFile('.png', function(filename){
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

    findFile('.exe', function(filename){
        if(filename)
        {
            fs.rename(filename,
                exec_dir + year + '/' + projectname + '/'+ projectname + '.exe',
                function(err){
                    if(err){
                        console.log(err);
                    }
                });
            upload_exec = projectname;
        }
    });

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

function findFile(extension, cb){
    var file = fs.readdirSync('tmp/');
    for(var i = 0; i < file.length; i++){
        var filename = path.join('tmp/', file[i]);
        if(filename.indexOf(extension) >= 0){
            cb(filename);
        }
    }
}
module.exports = router;
