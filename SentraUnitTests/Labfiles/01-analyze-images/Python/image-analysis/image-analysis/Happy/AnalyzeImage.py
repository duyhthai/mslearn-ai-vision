import unittest
from unittest.mock import patch, MagicMock
from your_module import AnalyzeImage  # Replace 'your_module' with the actual module name

class TestAnalyzeImage(unittest.TestCase):

    @patch('your_module.load_dotenv')  # Replace 'your_module' with the actual module name
    @patch('your_module.Image.open')
    def test_analyze_image_success(self, mock_open, mock_load):
        mock_image = MagicMock()
        mock_open.return_value = mock_image
        mock_cv_client = MagicMock()

        result = AnalyzeImage('test.jpg', None, mock_cv_client)

        self.assertEqual(result, None)
        mock_open.assert_called_once_with('test.jpg')
        mock_cv_client.analyze_image.assert_called_once_with(mock_image)

    @patch('your_module.load_dotenv')
    @patch('your_module.Image.open')
    def test_analyze_image_file_not_found(self, mock_open, mock_load):
        mock_open.side_effect = FileNotFoundError

        with self.assertRaises(FileNotFoundError):
            AnalyzeImage('nonexistent.jpg', None, MagicMock())

    @patch('your_module.load_dotenv')
    @patch('your_module.Image.open')
    def test_analyze_image_exception(self, mock_open, mock_load):
        mock_open.side_effect = Exception("Test exception")

        with self.assertRaises(Exception) as context:
            AnalyzeImage('error.jpg', None, MagicMock())

        self.assertIn("Test exception", str(context.exception))

if __name__ == '__main__':
    unittest.main()