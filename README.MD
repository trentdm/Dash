#Dash#
Dash is a webapp For aggregating data from a google spreadsheet, fed by google forms, and displaying the information in a useful format.

##Intent##
Dash is a small personal project, designed to help aggregate knowledge collected by dozens of individuals and their experiences.

##Technology##
Dash front-end is built upon AngularJS and AngularUI.
The server is built using ServiceStack. I am currently using ServiceStack.Ormlite for back-end persistence, but may change the database layer to something more customized, due to personal preference over ORMs in general.

##Api and Logging##
The restful api can be viewed at /metadata.

##Open Source##
Dash is an open source project, licensed under the Apache License.

##ToDo##
* Make more extensible for other users.
* Add auth back in.
* Investigate a integrated bundler to minimize payloads (MVC was good at this, or https://github.com/ServiceStack/Bundler if sticking with SS)
* Change metadata, auth, register, etc., paths to /api/{service}
* Customize metadata page, https://github.com/ServiceStack/ServiceStack/wiki/Metadata-page
* Use modernizr to detect if a client's browser may be unsupported (and notify with alert)