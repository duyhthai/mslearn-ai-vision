import unittest
from unittest.mock import patch, mock_open
from io import StringIO
import sys

class TestMain(unittest.TestCase):

    @patch('builtins.input', return_value='1')
    def test_main_read_api(self, mock_input):
        # Redirect stdout to capture printed output
        captured_output = StringIO()
        sys.stdout = captured_output
        
        # Mocking environment variables
        with patch.dict(os.environ, {'AI_SERVICE_ENDPOINT': 'http://fakeendpoint.com', 'AI_SERVICE_KEY': 'fakekey'}):
            with patch('main.main'):
                main()

        # Restore stdout
        sys.stdout = sys.__stdout__
        
        # Verify output
        expected_output = '\n1: Use Read API for image (Lincoln.jpg)\n2: Read handwriting (Note.jpg)\nAny other key to quit\n'
        expected_output += 'Enter a number:'
        self.assertIn(expected_output, captured_output.getvalue())

    @patch('builtins.input', return_value='2')
    def test_main_handwriting(self, mock_input):
        # Redirect stdout to capture printed output
        captured_output = StringIO()
        sys.stdout = captured_output
        
        # Mocking environment variables
        with patch.dict(os.environ, {'AI_SERVICE_ENDPOINT': 'http://fakeendpoint.com', 'AI_SERVICE_KEY': 'fakekey'}):
            with patch('main.main'):
                main()

        # Restore stdout
        sys.stdout = sys.__stdout__
        
        # Verify output
        expected_output = '\n1: Use Read API for image (Lincoln.jpg)\n2: Read handwriting (Note.jpg)\nAny other key to quit\n'
        expected_output += 'Enter a number:'
        self.assertIn(expected_output, captured_output.getvalue())

    @patch('builtins.input', return_value='3')
    def test_main_quit(self, mock_input):
        # Redirect stdout to capture printed output
        captured_output = StringIO()
        sys.stdout = captured_output
        
        # Mocking environment variables
        with patch.dict(os.environ, {'AI_SERVICE_ENDPOINT': 'http://fakeendpoint.com', 'AI_SERVICE_KEY': 'fakekey'}):
            with patch('main.main'):
                main()

        # Restore stdout
        sys.stdout = sys.__stdout__
        
        # Verify output
        expected_output = '\n1: Use Read API for image (Lincoln.jpg)\n2: Read handwriting (Note.jpg)\nAny other key to quit\n'
        expected_output += 'Enter a number:'
        self.assertIn(expected_output, captured_output.getvalue())

if __name__ == '__main__':
    unittest.main()