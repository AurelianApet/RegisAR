<?php
	require "conf/config.php";	
	require "conf/common.php";
	try {
		$conn = new PDO("mysql:host=$host;dbname=$dbname", $username, $password);
		$conn->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
		$stmt = $conn->prepare("SELECT * FROM business order by id");
		$stmt->execute();
		$result = array();
		$buses = $stmt->fetchAll();
		foreach($buses as $bus){
			array_push($result, array('id'=>$bus[0], 'name'=>$bus[1], 'boffer'=>$bus[2], 'lat'=>$bus[3], 'lng'=>$bus[4], 'category'=>$bus[5], 'status'=>$bus[6]));
		}
		echojson($result);
	}
	catch(PDOException $e) {
		echo "Error: " . $e->getMessage();
	}
	$conn = null;
?>