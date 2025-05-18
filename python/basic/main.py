__name__ = 'kamil'
def is_palindrome(word):
    """
    Checks if a word is a palindrome.
    :param word:
    :return:
    """
    if len(word) < 2:
        return True
    else:
        return word[0] == word[-1] and is_palindrome(word[1:-1])