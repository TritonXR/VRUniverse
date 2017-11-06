var express = require('express');
var router = express.Router();
var fs = require("fs");

function readFiles(dirname, onFileContent, onError) {
    fs.readdirSync(dirname, function (err, filenames) {
        if (err) {
            onError(err);
            return;
        }

        filenames
            .filter(function (filename) { return filename.substr(-5) === '.json'; })
            .forEach(function (filename) {
                console.log(dirname + filename);
                fs.readFileSync(dirname + filename, 'utf-8', function (err, content) {
                    if (err) {
                        onError(err);
                        return;
                    }
                    onFileContent(filename, content);
                });
            });
    });
}

/* GET users listing. */

router.get('/', function (req, res, next) {
    var data = {};
    readFiles('./data/VRClubUniverseData', function (filename, content) {
        var year = filename.spilt(".")[0];
        data[year] = content;
    }, function (err) { console.err(err); return; })
    console.log(data);
    res.render('projects', {
        "data": JSON.stringify(data)
    });
});
//{ "data" : JSON.stringify(data) }
module.exports = router;
