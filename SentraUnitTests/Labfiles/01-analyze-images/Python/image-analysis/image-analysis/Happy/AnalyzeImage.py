# This file contains unit tests for the AnalyzeImage function from the source code.

import unittest
from unittest.mock import patch, MagicMock
from io import BytesIO

class TestAnalyzeImage(unittest.TestCase):

    @patch('azure.cognitiveservices.vision.computervision.ImageAnalysisClient')
    def test_analyze_image_with_valid_input(self, mock_cv_client):
        # Arrange
        image_filename = 'test_image.jpg'
        image_data = b'\x89PNG\r\n\x1a\n\x00\x00\x00\rIHDR\x00\x00\x00\x01\x00\x00\x00\x01\x08\x02\x00\x00\x00\x90wS\xde\x00\x00\x00\x0cIDATx\xdac\xf8\xff?\x00\x01\x01\xfe\x00\x00\x00\x00IEND\xaeB`\x82'
        mock_cv_client.analyze_image.return_value = {'description': {'captions': [{'text': 'Test image', 'confidence': 1.0}]}}

        # Act
        result = AnalyzeImage(image_filename, image_data, mock_cv_client)

        # Assert
        self.assertEqual(result['description']['captions'][0]['text'], 'Test image')

    @patch('azure.cognitiveservices.vision.computervision.ImageAnalysisClient')
    def test_analyze_image_with_invalid_image_data(self, mock_cv_client):
        # Arrange
        image_filename = 'test_image.jpg'
        image_data = b'invalid_image_data'
        mock_cv_client.analyze_image.side_effect = Exception('Invalid image data')

        # Act & Assert
        with self.assertRaises(Exception) as context:
            AnalyzeImage(image_filename, image_data, mock_cv_client)
        self.assertIn('Invalid image data', str(context.exception))

    @patch('azure.cognitiveservices.vision.computervision.ImageAnalysisClient')
    def test_analyze_image_with_no_description(self, mock_cv_client):
        # Arrange
        image_filename = 'test_image.jpg'
        image_data = b'\x89PNG\r\n\x1a\n\x00\x00\x00\rIHDR\x00\x00\x00\x01\x00\x00\x00\x01\x08\x02\x00\x00\x00\x90wS\xde\x00\x00\x00\x0cIDATx\xdac\xf8\xff?\x00\x01\x01\xfe\x00\x00\x00\x00IEND\xaeB`\x82'
        mock_cv_client.analyze_image.return_value = {}

        # Act & Assert
        with self.assertRaises(KeyError) as context:
            AnalyzeImage(image_filename, image_data, mock_cv_client)
        self.assertIn("'description'", str(context.exception))

if __name__ == '__main__':
    unittest.main()