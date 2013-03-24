<?php
// This file is used as an abstraction layer for the traqpaq C# application. Login details are passed via the $_POST parameter
// and sent to the MySQL databse.
// Variables expected in the POST array are username/email, password, 
require_once 'functions.php';

// check if POST params are set
if (!isset($_POST['email']) || !isset($_POST['password'])) {
	header('HTTP/1.1 403 Forbidden');
	echo "<title>403 Forbidden</title>";
	echo "<h1>403 Forbidden</h1>";
	echo "You do not have permission to access this page.<hr>";
	die("<i>" . $_SERVER['SERVER_SOFTWARE'] . " Server at " . $_SERVER['SERVER_NAME'] . "</i>");
}

echo "Hello!";