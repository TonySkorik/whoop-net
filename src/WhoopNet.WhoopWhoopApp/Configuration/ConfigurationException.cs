namespace WhoopNet.WhoopWhoopApp.Configuration;

public class ConfigurationException(string key)
	: Exception($"Expected configuration {key} is not properly specified");