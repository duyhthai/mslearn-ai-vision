import unittest
from unittest.mock import patch, Mock
from io import BytesIO
from PIL import Image
import sys

class TestAnalyzeImage(unittest.TestCase):

    @patch('azure.cognitiveservices.vision.computervision.ComputerVisionClient')
    def test_analyze_image_invalid_cv_client(self, mock_cv_client):
        # Arrange
        image_filename = "test.jpg"
        image_data = BytesIO()
        mock_cv_client.return_value = None
        
        # Act & Assert
        with self.assertRaises(TypeError):
            AnalyzeImage(image_filename, image_data, mock_cv_client)

    @patch('azure.cognitiveservices.vision.computervision.ComputerVisionClient')
    def test_analyze_image_empty_image_data(self, mock_cv_client):
        # Arrange
        image_filename = "test.jpg"
        image_data = BytesIO(b'')
        mock_cv_client.return_value = Mock()
        
        # Act & Assert
        with self.assertRaises(ValueError):
            AnalyzeImage(image_filename, image_data, mock_cv_client)

    @patch('azure.cognitiveservices.vision.computervision.ComputerVisionClient')
    def test_analyze_image_nonexistent_file(self, mock_cv_client):
        # Arrange
        image_filename = "nonexistent.jpg"
        image_data = open(image_filename, 'rb')
        mock_cv_client.return_value = Mock()
        
        # Act & Assert
        with self.assertRaises(FileNotFoundError):
            AnalyzeImage(image_filename, image_data, mock_cv_client)

if __name__ == '__main__':
    unittest.main()