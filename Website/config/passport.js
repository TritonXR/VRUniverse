var passport = require('passport');
var User = require('../model/user');
var LocalStrategy = require('passport-local').Strategy;

passport.serializeUser(function(user, done) {
    done(null, user.id);
});

passport.deserializeUser(function(id, done) {
    User.findById(id, function(err, user) {
        done(err, user);
    });
});

passport.use('local.signup', new LocalStrategy({
    usernameField: 'username',
    passwordField: 'password',
    passReqToCallback: true
}, function(req, username, password, done) {
    User.findOne({'username': username}, function(err, user) {
        if(err) {
            return done(err);
        }
        if(user) {
            return done(null, false, {message: 'Username already in use'});
        }
        var newUser = new User();
        newUser.username = username;
        newUser.password = password;
        newUser.save(function(err, result) {
            if(err) {
                return done(err);
            }
            return done(null, newUser);
        });

    });
}));

passport.use('local.signin', new LocalStrategy({
    usernameField: 'username',
    passwordField: 'passport',
    passReqToCallback: true
}, function(req, uesrname, password, done) {
    User.findOne({'username': username}, function(err, user) {
        if(err) {
            return done(err);
        }
        if(!user) {
            return done(null, false, {message: 'failed'});
        }
        if(!user.validPassword(password)) {
            return done(null, false, {message: 'failed'});
        }
        return done(null, user);
    });
}));
