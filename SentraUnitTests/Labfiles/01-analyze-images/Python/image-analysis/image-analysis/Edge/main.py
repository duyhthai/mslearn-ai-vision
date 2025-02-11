# UnitTests.py
import unittest
from unittest.mock import patch, mock_open
from io import StringIO
from src.main import main  # Adjust the path according to your project structure

class TestMain(unittest.TestCase):

    @patch('builtins.open', new_callable=mock_open)
    def test_load_dotenv_with_valid_file(self, mock_open):
        mock_open.return_value.__iter__.return_value = ["KEY=value"]
        with patch.dict(os.environ, {}, clear=True):
            main()
            self.assertIn("KEY", os.environ)

    @patch('builtins.open', side_effect=FileNotFoundError)
    def test_load_dotenv_with_invalid_file(self, mock_open):
        with patch.dict(os.environ, {}, clear=True):
            main()
            self.assertNotIn("KEY", os.environ)

    @patch('azure.cognitiveservices.vision.computervision.ComputerVisionClient')
    def test_main_with_azure_client_creation(self, mock_cv_client):
        with patch.dict(os.environ, {"AZURE_KEY": "key"}, clear=True):
            main()
            mock_cv_client.assert_called_once_with(endpoint="https://westcentralus.api.cognitive.microsoft.com/", subscription_key="key")

if __name__ == '__main__':
    unittest.main()