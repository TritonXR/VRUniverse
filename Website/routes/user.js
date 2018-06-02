var express = require('express');
var router = express.Router();
var request = require('request-promise');
var octokit = require('@octokit/rest')({
    debug: true
})
var fs = require('fs');
var rq = require('request');
var progress = require('request-progress');
var WebSocketServer = require('ws').Server;
var unzip = require('unzip');
var multer = require('multer');
var path = require('path');
var vive = require('./vive-db.js')
var oculus = require('./oculus-db.js')


var storage = multer.diskStorage({
    destination: function(req, file, cb){
        cb(null, 'tmp/')
    },
    filename: function(req,file,cb){
        cb(null, file.originalname)
    }
});

var upload = multer({storage:storage});

const ORGNAME = 'TritonXR'

router.get('/signin', function(req, res, next) {
    res.render('user/signup');
});

router.get('/signedin', function(req, res, next) {
    let code = req.query.code;
    let token;
    let opts = {
        uri: 'https://github.com/login/oauth/access_token',
        qs: {
            client_id: process.env['GITHUB_CLIENT_ID'],
            client_secret: process.env['GITHUB_CLIENT_SECRET'],
            code: code
        },
        headers: {
            'User-Agent': 'Request-Promise'
        },
        json: true 
    };

    request(opts)
        .then((data) => {
            token = data.access_token;
            res.cookie('github_access_token', token);

            octokit.authenticate({
                type: 'oauth',
                token: token
            })

            octokit.users.getOrgMembership({org: ORGNAME})
                .then((res2) => {

                    if(res2.data.role == "admin" || process.env.IS_ADMIN == 1) {

                        //callback hell yay me
                        vive.getAllProjects(function(vive_data) {
                            oculus.getAllProjects(function(oculus_data) {
                                res.render('manage', {vive: vive_data, oculus: oculus_data});
                            });
                        });

                        return;
                    }

                    if (res2.data.role) {
                        vive.getAllTags((tags) => {
                            res.render('upload', {"tags" : tags});
                        });
                        return;
                    }
                    res.json({err: "You Are Not A Member Of UCSDVR!"})

                })
                .catch((err) => res.render('universe_err', {err: "You Have An Error: \n"+err.status}))
        })
});


router.post('/upload', upload.any(), function(req, res) {
    var year = req.body.year;
    var projectname = req.body.project;
    var upload_image = '';
    var upload_exec = '';
    console.log(req.body)

    var platform = req.body.platform;

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

    findFile('.zip', function(filename){
        if(filename)
        {
            fs.createReadStream(filename).pipe(unzip.Extract({ path: exec_dir + year + '/' + projectname + '/'+ projectname + '.exe' }));
            upload_exec = projectname;
        }
    });

    var tags = req.body.tags;

    if (typeof tags === 'string') {
        tags = [tags];
    }
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
                        res.json({err});
                        return;
                    }

                    db.createEntry(proj, function(status) {
                        res.send('Successfully Uploaded Project!');
                        res.render('upload_success',{upload_success:"Successfully Uploaded Project!"});
                    });
                });
            }
        }
    });
});

router.post('/remove', function(res, req, next) {
    console.log(req.body, req.query);
    res.json({"wow" : "wow"});
})

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
