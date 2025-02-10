import unittest
from unittest.mock import patch
from io import StringIO

class TestGetTextRead(unittest.TestCase):
    @patch('builtins.print')
    def test_get_text_read_invalid_image_type(self, mock_print):
        with self.assertRaises(TypeError):
            GetTextRead("invalid_image.txt")

    @patch('builtins.print')
    def test_get_text_read_nonexistent_image(self, mock_print):
        with self.assertRaises(FileNotFoundError):
            GetTextRead("nonexistent_image.jpg")

    @patch('builtins.print')
    def test_get_text_read_empty_image(self, mock_print):
        with open("empty_image.png", "wb") as f:
            f.write(b'\x89PNG\r\n\x1a\n\x00\x00\x00\rIHDR\x00\x00\x00\x01\x00\x00\x00\x01\x08\x02\x00\x00\x00\x90wS\xde\x00\x00\x00\x0cIDATx\xdac\xf8\xff?\x00\x00\x00\x05\x00\x01\x01\xfe\xfd\x00\x00\x00\x00IEND\xaeB`\x82')
        with self.assertRaises(Exception):
            GetTextRead("empty_image.png")
        os.remove("empty_image.png")

if __name__ == '__main__':
    unittest.main()