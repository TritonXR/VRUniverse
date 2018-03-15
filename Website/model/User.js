var mongoose = require('mongoose');
var Schema = mongoose.Schema;

var userSchema = new Schema({
    username: {type: String, required: true},
    password: {type: String, required: true}
});

userSchema.methods.validPassword = function(password) {
    console.log(this.password + " vs. " + password);
    return this.password===password;
}

module.exports = mongoose.model('User', userSchema);