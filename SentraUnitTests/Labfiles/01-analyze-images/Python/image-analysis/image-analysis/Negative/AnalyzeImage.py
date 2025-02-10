import unittest
from unittest.mock import patch
from io import BytesIO
from azure.cognitiveservices.vision.computervision import ComputerVisionClient
from azure.cognitiveservices.vision.computervision.models import VisualFeatureTypes
from msrest.authentication import CognitiveServicesCredentials

class TestAnalyzeImage(unittest.TestCase):

    @patch('azure.cognitiveservices.vision.computervision.ComputerVisionClient')
    def test_analyze_image_with_invalid_cv_client(self, mock_cv_client):
        with self.assertRaises(TypeError):
            AnalyzeImage('image.jpg', b'\x89PNG\r\n\x1a\n...', None)

    @patch('azure.cognitiveservices.vision.computervision.ComputerVisionClient')
    def test_analyze_image_with_empty_image_data(self, mock_cv_client):
        with self.assertRaises(ValueError):
            AnalyzeImage('image.jpg', b'', mock_cv_client())

    @patch('azure.cognitiveservices.vision.computervision.ComputerVisionClient')
    def test_analyze_image_with_non_image_file(self, mock_cv_client):
        with self.assertRaises(FileNotFoundError):
            AnalyzeImage('nonexistent.txt', b'\x89PNG\r\n\x1a\n...', mock_cv_client())

if __name__ == '__main__':
    unittest.main()