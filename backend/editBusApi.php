<?php
	require "conf/config.php";	
	require "conf/common.php";
	try {
		$conn = new PDO("mysql:host=$host;dbname=$dbname", $username, $password);
		$conn->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
		$bid = 0; $name = ""; $boffer = ""; $lat = 1000; $long = 1000; $category = -1; $status = -1;
		if(!isset($_POST['bid'])){
			echojson(array('success'=>0));exit;
		}
		$bid = $_POST['bid'];
		$sql = "UPDATE business SET ";
		$con = " WHERE id=" . $bid;
		$ct = false;
		if(isset($_POST['name'])){
			$name = $_POST['name'];
			$sql .= "name='" . $name . "'";
			$ct = true;
		}
		if(isset($_POST['boffer'])){
			$boffer = $_POST['boffer'];
			if($ct == true){
				$sql .= ", boffer='" . $boffer . "'";
			}else{
				$sql .= "boffer='" . $boffer . "'";
				$ct = true;
			}
		}
		if(isset($_POST['lat'])){
			$lat = $_POST['lat'];
			if($ct == true){
				$sql .= ", lat='" . $lat . "'";
			}else{
				$sql .= "lat='" . $lat . "'";
				$ct = true;
			}
		}
		if(isset($_POST['lng'])){
			$lng = $_POST['lng'];
			if($ct == true){
				$sql .= ", lng='" . $lng . "'";
			}else{
				$sql .= "lng='" . $lng . "'";
				$ct = true;
			}
		}
		if(isset($_POST['category'])){
			$category = $_POST['category'];
			if($category > -1){
				if($ct == true){
					$sql .= ", category=" . $category;
				}else{
					$sql .= "category=" . $category;
					$ct = true;
				}
			}
		}
		if(isset($_POST['status'])){
			$status = $_POST['status'];
			if($status > -1){
				if($ct == true){
					$sql .= ", status=" . $status;
				}else{
					$sql .= "status=" . $status;
					$ct = true;
				}
			}
		}
		if($ct == true){
			$sql .= $con;
		}else{
			echojson(array('success'=>0));
		}
		//echo $sql;exit;
		if($conn->prepare($sql)->execute()){
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