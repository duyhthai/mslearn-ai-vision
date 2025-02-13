import unittest
from unittest.mock import patch, Mock
from io import BytesIO
from PIL import Image

class TestAnalyzeImage(unittest.TestCase):

    @patch('azure.cognitiveservices.vision.computervision.ComputerVisionClient')
    def test_analyze_image_with_valid_input(self, mock_cv_client):
        # Arrange
        image_filename = "test.jpg"
        image_data = BytesIO(b'\x89PNG\r\n\x1a\n\x00\x00\x00\rIHDR\x00\x00\x00\x01\x00\x00\x00\x01\x08\x02\x00\x00\x00\x90wS\xde\x00\x00\x00\x0cIDATx\xdac\xf8\xff?\x00\x01\x00\x01\xfe\x00\x00\x00\x00IEND\xaeB`\x82')
        mock_cv_client.analyze_image.return_value = Mock()

        # Act
        result = AnalyzeImage(image_filename, image_data, mock_cv_client)

        # Assert
        mock_cv_client.analyze_image.assert_called_once_with(image_data)
        self.assertIsNone(result)

    @patch('azure.cognitiveservices.vision.computervision.ComputerVisionClient')
    def test_analyze_image_with_empty_image_data(self, mock_cv_client):
        # Arrange
        image_filename = "test.jpg"
        image_data = BytesIO(b'')
        mock_cv_client.analyze_image.side_effect = Exception("Empty image data")

        # Act & Assert
        with self.assertRaises(Exception) as context:
            AnalyzeImage(image_filename, image_data, mock_cv_client)
        self.assertIn("Empty image data", str(context.exception))

    @patch('azure.cognitiveservices.vision.computervision.ComputerVisionClient')
    def test_analyze_image_with_invalid_image_format(self, mock_cv_client):
        # Arrange
        image_filename = "test.txt"
        image_data = BytesIO(b'This is not an image')
        mock_cv_client.analyze_image.side_effect = Exception("Invalid image format")

        # Act & Assert
        with self.assertRaises(Exception) as context:
            AnalyzeImage(image_filename, image_data, mock_cv_client)
        self.assertIn("Invalid image format", str(context.exception))

if __name__ == '__main__':
    unittest.main()