<?php
	require "conf/config.php";	
	require "conf/common.php";
	try {
		$conn = new PDO("mysql:host=$host;dbname=$dbname", $username, $password);
		$conn->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
		$bid = $_POST['bid'];
		if(@current($conn->query("delete from business where id=" . $bid))){
			echojson(array('success'=>1));
		}else{
			echojson(array('success'=>0));
		}
	}
	catch(PDOException $e) {
		echo "Error: " . $e->getMessage();
	}
	$conn = null;
?>