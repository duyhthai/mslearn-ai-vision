import unittest
from unittest.mock import patch
from your_module import GetTextRead  # Replace 'your_module' with the actual module name

class TestGetTextRead(unittest.TestCase):
    @patch('os.getenv')
    def test_get_text_read_with_valid_image(self, mock_getenv):
        mock_getenv.return_value = 'path/to/image.jpg'
        result = GetTextRead('image.jpg')
        self.assertEqual(result, None)

    @patch('os.getenv', return_value=None)
    def test_get_text_read_without_image_path(self, mock_getenv):
        with self.assertRaises(SystemExit):
            GetTextRead(None)

    @patch('os.getenv', return_value='')
    def test_get_text_read_with_empty_image_path(self, mock_getenv):
        with self.assertRaises(SystemExit):
            GetTextRead('')

if __name__ == '__main__':
    unittest.main()