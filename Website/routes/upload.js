var express = require('express');
var router = express.Router();
var formidable = require("formidable");

var fs = require('fs');
// var multer = require('multer');
/*
var storage = multer.diskStorage({
    destination: function (req, file, cb) {
        cb(null,'./data' )
    },
    filename: function (req, file, cb) {
        cb(null, file.orginalname)
    }
});

var upload = multer({storage: storage});
*/
/* GET users listing. */

router.get('/', function(req, res, next) {
    res.render('upload');
});

router.post('/', function(req, res) {

    res.send(JSON.stringify(req.body));

    //res.send(req.files);
    //console.log(req.files, 'files');
    //console.log(req.body, 'body');
    /*var year = 2016;
    var json_dir = "./data/VRClubUniverseData/";
    var content = null;
    fs.readFile(json_dir + year + '.json', 'utf-8', function (err, file_content) {
        if (err) {
            console.log(err);
            return;
        }
        content = JSON.parse(file_content);
        console.log(content);

              processForm(req, res, function (data) {
            res.send(data);
        });

    });*/
});

module.exports = router;
/*
function processForm(req,res, callback){

    var form = new formidable.IncomingForm();
    form.parse(req, function(err, fields, files) {

        // mkdir if not exist
        var year = 2016;
        var json_dir = "./data/VRClubUniverseData/";
        var exec_dir = "./data/VRClubUniverseData/VR_Demos/";
        // if(!fs.existsSync(exec_dir + year)) {fs.mkdirSync(exec_dir + year);}
        // if(!fs.existsSync(exec_dir + year + '/' + projectname)) {fs.mkdirSync(exec_dir + year + '/' + projectname);}

        // parse texts
        var content = null;
        fs.readFile(json_dir + year + '.json', 'utf-8', function(err, file_content)
        {
            if(err) {
                console.log(err);
                return;
            }
            content = JSON.parse(file_content);
            console.log(content);


            if(content != null) {
                callback(content);
            }
            res.end('');
        });
        // parse non-text stuff
        /*
        var image = req.body.picture;
        fs.writeFile(exec_dir + year + '/' + projectname + '/' + projectname + '.jpg' , image, function(err){
            if(err){
                console.log(err);
            }
        });
        var exec = req.body.exec;
        fs.writeFile(exec_dir + year + '/' + projectname + '/' + projectname + '.exe', exec, function(err){
            if(err){
                console.log(err);
            }
        })

    });
}*/

