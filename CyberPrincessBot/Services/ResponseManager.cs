using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSafeBot.Services
{
    internal class ResponseManager
    {

            // For each topic, we store:
            // - neutral responses (standard)
            // - curious responses
            // - worried responses
            // - follow-up responses ("tell me more" and "another tip")

            public Dictionary<string, List<string>> NeutralResponses { get; } = new();
            public Dictionary<string, List<string>> CuriousResponses { get; } = new();
            public Dictionary<string, List<string>> WorriedResponses { get; } = new();
            public Dictionary<string, List<string>> FollowUpResponses { get; } = new();

            public ResponseManager()
            {
                // ----- PHISHING -----
                NeutralResponses["phishing"] = new List<string>
            {
                "🛡️ Phishing is when attackers send fake emails or messages to steal your personal info. Always check the sender's address.",
                "🎣 Phishing attacks often create urgency – 'Your account will be closed!' Don't click suspicious links.",
                "📧 If an email asks for your password or credit card, it's likely phishing. Legit companies never ask that way."
            };

                CuriousResponses["phishing"] = new List<string>
            {
                "🤔 Great curiosity! Phishing can also happen via SMS ('smishing') or phone calls ('vishing'). Always verify the source.",
                "🔍 Since you're curious: phishers often use fake websites that look identical to real ones. Hover over links to see the real URL.",
                "💡 Fun fact: some phishing emails have spelling mistakes – that's a red flag. Legit companies proofread."
            };

                WorriedResponses["phishing"] = new List<string>
            {
                "😟 I understand your worry. Here's a comforting tip: enable two‑factor authentication (2FA) – even if they steal your password, they can't get in.",
                "💖 It's normal to be worried. Remember: never share your OTP or login details. Use a password manager to avoid fake sites.",
                "🛡️ Being worried keeps you alert. A simple rule: if something feels off, don't click. Contact the company directly using a known number."
            };

                FollowUpResponses["phishing"] = new List<string>
            {
                "📌 Another anti‑phishing tip: look for HTTPS and the padlock icon in the address bar before entering any info.",
                "🔁 Here's one more: if you receive a suspicious email, report it to your IT team or forward it to report@phishing.gov (in many countries).",
                "⚠️ Bonus tip: Phishing attacks increased by 60% last year. Always be skeptical of unexpected messages."
            };

                // ----- PASSWORD -----
                NeutralResponses["password"] = new List<string>
            {
                "🔐 Use a strong password: mix uppercase, lowercase, numbers, and symbols. Avoid 'password123'!",
                "🗝️ Never reuse passwords across sites. Use a password manager to generate and store unique ones.",
                "🛡️ Enable 2FA for an extra layer of security – even if your password leaks."
            };
                CuriousResponses["password"] = new List<string>
            {
                "🤓 Since you're curious: hackers use 'brute force' attacks trying millions of combinations. Long passwords (12+ chars) are best.",
                "📚 Fun fact: the most common password is still '123456' – don't be like that! Use a passphrase like 'PurpleDragon$RunsFast!'",
                "💡 A curious mind wants to know: password managers like Bitwarden or LastPass can create and fill complex passwords for you."
            };
                WorriedResponses["password"] = new List<string>
            {
                "😟 Worried about password security? That's smart. Start by changing any reused passwords today.",
                "💖 It's okay to feel worried. Remember: a password manager can help you only remember one master password.",
                "🛡️ If you're anxious about being hacked, enable 2FA everywhere it's offered – it blocks 99.9% of account takeovers."
            };
                FollowUpResponses["password"] = new List<string>
            {
                "🔁 Another tip: change your password if a service you use announces a data breach. Use 'Have I Been Pwned' to check.",
                "📌 Bonus: avoid using personal info like your pet's name or birthdate. Those are easy to guess from social media.",
                "⚠️ One more: use a unique password for your email account – if that's hacked, attackers can reset all your other passwords."
            };

                // ----- SCAM -----
                NeutralResponses["scam"] = new List<string>
            {
                "💰 Scams try to trick you into sending money or personal info. If it sounds too good to be true, it probably is.",
                "📞 'You've won a prize!' or 'Your computer has a virus!' – common scam openers. Hang up or delete.",
                "🛡️ Never pay upfront for a promised prize. Legit lotteries don't ask for fees."
            };
                CuriousResponses["scam"] = new List<string>
            {
                "🤓 Curious about how scammers operate? They create urgency – 'Act now or lose everything!' That's a red flag.",
                "🔍 Some scams impersonate your bank via SMS. Always call the bank using the number on your card, not the one in the message.",
                "💡 Fun insight: romance scams are on the rise – people fake affection to steal money. Never send money to someone you haven't met."
            };
                WorriedResponses["scam"] = new List<string>
            {
                "😟 Feeling worried about scams is completely valid. A simple rule: never share your OTP or login details with anyone.",
                "💖 Your worry protects you. Remember: legitimate companies will never call you and ask for your password or credit card number.",
                "🛡️ If you're scared you've been scammed, contact your bank immediately and report to local authorities."
            };
                FollowUpResponses["scam"] = new List<string>
            {
                "🔁 Another tip: install a call blocker app if you get many spam calls. Many are free.",
                "📌 Bonus: if you receive a suspicious email, don't reply – just delete it and block the sender.",
                "⚠️ One more: scammers often use fake invoices. Always check your bank statements regularly."
            };

                // ----- PRIVACY -----
                NeutralResponses["privacy"] = new List<string>
            {
                "👁️ Protect your privacy: review app permissions, use a VPN on public Wi‑Fi, and limit what you share on social media.",
                "🔒 Clear your cookies and browsing data regularly. Also, turn off location tracking for apps that don't need it.",
                "📱 Cover your webcam when not in use. And use strong, unique answers for security questions."
            };
                CuriousResponses["privacy"] = new List<string>
            {
                "🤓 Since you're curious: companies collect data to sell ads. Use privacy-focused browsers like Brave or Firefox with tracking protection.",
                "🔍 Did you know? Your phone's microphone isn't always listening – but apps can request access. Check your settings!",
                "💡 Fun: using a pseudonym online can help protect your real identity. Be careful with photos that show your home or workplace."
            };
                WorriedResponses["privacy"] = new List<string>
            {
                "😟 Worried about privacy? That's healthy. Start by turning off personalized ads on Google and social media.",
                "💖 Your feelings matter. Use a password manager and encrypt sensitive files on your computer.",
                "🛡️ If you're anxious, consider using a privacy screen protector on your laptop when working in public."
            };
                FollowUpResponses["privacy"] = new List<string>
            {
                "🔁 Another privacy tip: regularly review your social media privacy settings – set profiles to 'friends only'.",
                "📌 Bonus: use two‑factor authentication on your email and social accounts to prevent takeover.",
                "⚠️ One more: be careful with quizzes that ask for your pet's name or birthdate – those are security questions!"
            };

                // ----- SAFE BROWSING -----
                NeutralResponses["safe browsing"] = new List<string>
            {
                "🌐 Only visit trusted websites. Look for HTTPS and avoid clicking on pop‑up ads.",
                "🛡️ Use browser extensions like uBlock Origin to block malicious ads and trackers.",
                "🔍 Keep your browser updated – security patches fix vulnerabilities."
            };
                CuriousResponses["safe browsing"] = new List<string>
            {
                "🤓 Curious about safe browsing? Check if a link is safe by hovering over it – the real URL appears at the bottom.",
                "🔍 Fun: Google Safe Browsing protects Chrome and Firefox. You can also use VirusTotal to scan suspicious links.",
                "💡 A curious tip: use a search engine like DuckDuckGo that doesn't track your searches."
            };
                WorriedResponses["safe browsing"] = new List<string>
            {
                "😟 Worried about malicious websites? Install an antivirus that includes web protection.",
                "💖 It's good to be careful. Never download software from pop‑ups – always go to the official site.",
                "🛡️ If you feel unsafe, use a sandbox or a virtual machine for testing unknown sites."
            };
                FollowUpResponses["safe browsing"] = new List<string>
            {
                "🔁 Another tip: clear your browser's cache and history regularly to remove tracking cookies.",
                "📌 Bonus: avoid using public computers for online banking or shopping.",
                "⚠️ One more: enable 'Do Not Track' in your browser settings (though not all sites respect it)."
            };

                // ----- MALWARE -----
                NeutralResponses["malware"] = new List<string>
            {
                "🦠 Malware includes viruses, ransomware, and spyware. Always keep your antivirus updated.",
                "⚠️ Don't download files from untrusted sources. If an email attachment looks suspicious, don't open it.",
                "🛡️ Use a standard (non‑admin) user account for daily tasks – malware can't install easily then."
            };
                CuriousResponses["malware"] = new List<string>
            {
                "🤓 Curious about malware? Ransomware locks your files and demands payment. Always keep backups!",
                "🔍 Fun fact: some malware hides inside pirated software or 'free' game cracks.",
                "💡 A curious learner: you can test suspicious files in a virtual machine to keep your main system safe."
            };
                WorriedResponses["malware"] = new List<string>
            {
                "😟 Worried about malware? Run a full antivirus scan and use Windows Defender or Malwarebytes.",
                "💖 Your caution is good. Never plug in unknown USB drives – they can contain auto‑running malware.",
                "🛡️ If you're anxious, consider using a non‑Windows OS like Linux for sensitive tasks."
            };
                FollowUpResponses["malware"] = new List<string>
            {
                "🔁 Another malware tip: keep your operating system and all software updated – many updates patch security holes.",
                "📌 Bonus: use an ad blocker – malicious ads can install malware without you clicking (drive‑by downloads).",
                "⚠️ One more: backup your important files to an external drive or cloud. That protects you from ransomware."
            };
            }

            public string GetResponse(string topic, string sentiment, bool isFollowUp = false)
            {
                topic = topic.ToLower();
                var dict = isFollowUp ? FollowUpResponses :
                           (sentiment == "curious" ? CuriousResponses :
                           (sentiment == "worried" ? WorriedResponses : NeutralResponses));

                if (dict.ContainsKey(topic) && dict[topic].Count > 0)
                {
                    var list = dict[topic];
                    var rand = new System.Random();
                    return list[rand.Next(list.Count)];
                }
                // fallback
                return "Let me share a general tip: stay alert and think before you click!";
            }
        }
    }


