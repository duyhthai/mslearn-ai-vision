# Import necessary libraries
import unittest
from unittest.mock import patch, MagicMock
from io import StringIO
import os

class TestMain(unittest.TestCase):

    @patch('azure.ai.vision.computer_vision.ComputerVisionClient')
    def test_main_with_valid_input(self, MockComputerVisionClient):
        # Arrange
        mock_cv_client = MagicMock()
        MockComputerVisionClient.return_value = mock_cv_client
        
        # Mock environment variables
        os.environ['AZURE_COMPUTER_VISION_ENDPOINT'] = 'https://example.com'
        os.environ['AZURE_COMPUTER_VISION_KEY'] = 'key'

        # Mock input arguments
        sys.argv = ['main.py', 'path/to/image.jpg']

        # Mock standard output
        original_stdout = sys.stdout
        sys.stdout = StringIO()

        # Act
        from your_module_name import main  # Replace 'your_module_name' with the actual module name
        main()

        # Assert
        mock_cv_client.analyze_image.assert_called_once_with(
            image_path='path/to/image.jpg',
            visual_features=['Description']
        )
        self.assertIn('Image description:', sys.stdout.getvalue())

        # Clean up
        sys.stdout = original_stdout

    @patch('azure.ai.vision.computer_vision.ComputerVisionClient')
    def test_main_with_invalid_endpoint(self, MockComputerVisionClient):
        # Arrange
        MockComputerVisionClient.side_effect = Exception("Invalid endpoint")

        # Mock environment variables
        os.environ['AZURE_COMPUTER_VISION_ENDPOINT'] = 'invalid_endpoint'
        os.environ['AZURE_COMPUTER_VISION_KEY'] = 'key'

        # Mock input arguments
        sys.argv = ['main.py', 'path/to/image.jpg']

        # Mock standard error
        original_stderr = sys.stderr
        sys.stderr = StringIO()

        # Act
        from your_module_name import main  # Replace 'your_module_name' with the actual module name
        main()

        # Assert
        mock_cv_client.analyze_image.assert_not_called()
        self.assertIn('Error:', sys.stderr.getvalue())

        # Clean up
        sys.stderr = original_stderr

    @patch('azure.ai.vision.computer_vision.ComputerVisionClient')
    def test_main_with_empty_image_path(self, MockComputerVisionClient):
        # Arrange
        mock_cv_client = MagicMock()
        MockComputerVisionClient.return_value = mock_cv_client
        
        # Mock environment variables
        os.environ['AZURE_COMPUTER_VISION_ENDPOINT'] = 'https://example.com'
        os.environ['AZURE_COMPUTER_VISION_KEY'] = 'key'

        # Mock input arguments
        sys.argv = ['main.py', '']

        # Mock standard error
        original_stderr = sys.stderr
        sys.stderr = StringIO()

        # Act
        from your_module_name import main  # Replace 'your_module_name' with the actual module name
        main()

        # Assert
        mock_cv_client.analyze_image.assert_not_called()
        self.assertIn('Error:', sys.stderr.getvalue())

        # Clean up
        sys.stderr = original_stderr

if __name__ == '__main__':
    unittest.main()