import unittest
from unittest.mock import patch, MagicMock
from your_module import AnalyzeImage

class TestAnalyzeImage(unittest.TestCase):

    @patch('your_module.load_dotenv')
    def test_invalid_image_filename(self, mock_load_dotenv):
        result = AnalyzeImage(None, None, None)
        self.assertIsNone(result)

    @patch('your_module.Image.open')
    def test_invalid_image_data(self, mock_open):
        mock_open.side_effect = IOError("Invalid image data")
        result = AnalyzeImage("test.jpg", None, None)
        self.assertIsNone(result)

    @patch('your_module.cv_client.analyze_image')
    def test_cv_client_failure(self, mock_analyze_image):
        mock_analyze_image.side_effect = Exception("CV client error")
        result = AnalyzeImage("test.jpg", "image_data", None)
        self.assertIsNone(result)

if __name__ == '__main__':
    unittest.main()