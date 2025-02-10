import unittest
from unittest.mock import patch, Mock
from your_module import AnalyzeImage  # Replace 'your_module' with the actual module name

class TestAnalyzeImage(unittest.TestCase):
    @patch('your_module.load_dotenv')  # Replace 'your_module' with the actual module name
    def test_analyze_image_with_invalid_cv_client(self, mock_load_dotenv):
        mock_cv_client = None
        with self.assertRaises(TypeError) as context:
            AnalyzeImage('image.jpg', b'image_data', mock_cv_client)
        self.assertIn("cv_client must be an instance of Azure Cognitive Services client", str(context.exception))

    @patch('your_module.load_dotenv')
    def test_analyze_image_with_empty_image_data(self, mock_load_dotenv):
        with self.assertRaises(ValueError) as context:
            AnalyzeImage('image.jpg', b'', Mock())
        self.assertIn("image_data cannot be empty", str(context.exception))

    @patch('your_module.load_dotenv')
    def test_analyze_image_with_nonexistent_image_file(self, mock_load_dotenv):
        with open('nonexistent_image.jpg', 'w') as f:
            pass
        with self.assertRaises(FileNotFoundError) as context:
            AnalyzeImage('nonexistent_image.jpg', b'image_data', Mock())
        self.assertIn("No such file or directory", str(context.exception))
        os.remove('nonexistent_image.jpg')

if __name__ == '__main__':
    unittest.main()