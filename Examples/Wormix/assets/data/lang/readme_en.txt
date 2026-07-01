XMl files contains all text that we use in Dialogs

1. In the file you should replace the content of the "value" attribute and CDATA section with the translation

Example 1
<message name="LOADING" value="Loading..."/>
on russian
<message name="LOADING" value="Загрузка..."/>

Example 2

<message name="MAIN_SERVER_CONNECTION_CLOSED">
        <![CDATA[Connection to server is lost. <br/>Please update the page.]]>
</message>
on russian
<message name="MAIN_SERVER_CONNECTION_CLOSED">
        <![CDATA[Игра потеряла соединение с сервером. <br/>Пожалуйста, обновите страницу.]]>
</message>

2.It is necessary to save Uppercase letters as in the initial version 

3. In translated text all words in {} should be saved as is. This words will be replaced with appropriate values
Example 3
<message name="EXP_QTIP" value="Current experience: {cur}/{max}"/>
on russian
<message name="EXP_QTIP" value="Текущий опыт: {cur}/{max}"/>

in game: Current experience:10/50 

4. All html tags like: <font>, <br>, <b> should be saved on their places

file content description
1. messages        - all dialog texts
2. items.messages  - weapons and hat descrions 
3. help            - help messages    
5. taunts - funny phrases that are shown during the battle to comment the event happened in the battle. This could be proverbs or expressions, that are popular in Korea
we have not yet attracted a professional translator to prepare english version of taunts, so I provide you with a literal translation from Russian into English to understand the meaning of the phrases(taunts), but I think for the Korean version you shouldn't translate them. But you should prepare fun phrases that are familiar and ridiculous for the Korean users



