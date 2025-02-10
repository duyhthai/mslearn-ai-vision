import unittest
from unittest.mock import patch
from io import StringIO
import sys

# Mocking external libraries
@patch('azure.cognitiveservices.vision.computervision.ComputerVisionClient')
class TestMain(unittest.TestCase):
    def setUp(self):
        # Redirect stdout to capture print statements
        self.old_stdout = sys.stdout
        self.test_stdout = StringIO()
        sys.stdout = self.test_stdout

    def tearDown(self):
        # Restore original stdout
        sys.stdout = self.old_stdout

    @patch('os.getenv', return_value='fake_api_key')
    def test_main_with_valid_input(self, mock_getenv):
        # Arrange
        sys.argv = ['main.py']
        
        # Act
        main()
        
        # Assert
        self.assertIn("Azure Computer Vision Client initialized successfully", self.test_stdout.getvalue())
        mock_getenv.assert_called_once_with('AZURE_COMPUTER_VISION_KEY')

if __name__ == '__main__':
    unittest.main()