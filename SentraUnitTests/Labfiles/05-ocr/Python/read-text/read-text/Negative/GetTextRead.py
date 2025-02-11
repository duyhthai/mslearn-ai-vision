import unittest
from unittest.mock import patch
from io import StringIO
from PIL import Image

class TestGetTextRead(unittest.TestCase):

    @patch('builtins.print')
    def test_get_text_read_with_nonexistent_image(self, mock_print):
        with self.assertRaises(FileNotFoundError):
            GetTextRead("nonexistent_image.jpg")

    @patch('builtins.print')
    def test_get_text_read_with_empty_string(self, mock_print):
        with self.assertRaises(ValueError):
            GetTextRead("")

    @patch('builtins.print')
    def test_get_text_read_with_none(self, mock_print):
        with self.assertRaises(TypeError):
            GetTextRead(None)

if __name__ == '__main__':
    unittest.main()