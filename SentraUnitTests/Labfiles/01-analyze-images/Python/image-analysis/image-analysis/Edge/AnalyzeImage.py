import unittest
from unittest.mock import patch, MagicMock
from io import BytesIO
from azure.cognitiveservices.vision.computervision import ComputerVisionClient
from azure.cognitiveservices.vision.computervision.models import VisualFeatureTypes
from msrest.authentication import CognitiveServicesCredentials

class TestAnalyzeImage(unittest.TestCase):

    @patch('azure.cognitiveservices.vision.computervision.ComputerVisionClient')
    def test_analyze_image_with_valid_input(self, MockComputerVisionClient):
        # Arrange
        mock_cv_client = MockComputerVisionClient.return_value
        mock_cv_client.analyze_image_in_stream.return_value = {
            "description": {
                "captions": [
                    {"text": "A beautiful landscape", "confidence": 0.9}
                ]
            }
        }

        image_filename = 'test.jpg'
        image_data = BytesIO(b'\x89PNG\r\n\x1a\n\x00\x00\x00\rIHDR\x00\x00\x00\x01\x00\x00\x00\x01\x08\x02\x00\x00\x00\x90wS\xde\x00\x00\x00\x0cIDATx\xdac\xf8\xff\xff?\x00\x01\x05\xfe\xfe\xfe\x01\x00\x00\x00\x00IEND\xaeB`\x82')
        cv_client = ComputerVisionClient('subscription_key', CognitiveServicesCredentials('endpoint'))

        # Act
        result = AnalyzeImage(image_filename, image_data, cv_client)

        # Assert
        mock_cv_client.analyze_image_in_stream.assert_called_once_with(image_data, visual_features=[VisualFeatureTypes.description])
        self.assertIn("A beautiful landscape", result)

    @patch('azure.cognitiveservices.vision.computervision.ComputerVisionClient')
    def test_analyze_image_with_empty_image_data(self, MockComputerVisionClient):
        # Arrange
        mock_cv_client = MockComputerVisionClient.return_value
        image_filename = 'test.jpg'
        image_data = BytesIO(b'')
        cv_client = ComputerVisionClient('subscription_key', CognitiveServicesCredentials('endpoint'))

        # Act & Assert
        with self.assertRaises(Exception) as context:
            AnalyzeImage(image_filename, image_data, cv_client)
        self.assertTrue("No image data provided" in str(context.exception))

    @patch('azure.cognitiveservices.vision.computervision.ComputerVisionClient')
    def test_analyze_image_with_invalid_subscription_key(self, MockComputerVisionClient):
        # Arrange
        mock_cv_client = MockComputerVisionClient.return_value
        mock_cv_client.analyze_image_in_stream.side_effect = Exception("Invalid subscription key")
        image_filename = 'test.jpg'
        image_data = BytesIO(b'\x89PNG\r\n\x1a\n\x00\x00\x00\rIHDR\x00\x00\x00\x01\x00\x00\x00\x01\x08\x02\x00\x00\x00\x90wS\xde\x00\x00\x00\x0cIDATx\xdac\xf8\xff\xff?\x00\x01\x05\xfe\xfe\xfe\x01\x00\x00\x00\x00IEND\xaeB`\x82')
        cv_client = ComputerVisionClient('invalid_subscription_key', CognitiveServicesCredentials('endpoint'))

        # Act & Assert
        with self.assertRaises(Exception) as context:
            AnalyzeImage(image_filename, image_data, cv_client)
        self.assertTrue("Invalid subscription key" in str(context.exception))

if __name__ == '__main__':
    unittest.main()